import urllib.request
import urllib.parse
import http.cookiejar
import re
import sys

def safe_print(msg):
    try:
        print(msg)
    except UnicodeEncodeError:
        try:
            print(msg.encode('ascii', 'ignore').decode('ascii'))
        except Exception:
            pass

class SDMSTester:
    def __init__(self, base_url="http://localhost:5162"):
        self.base_url = base_url
        self.cj = http.cookiejar.CookieJar()
        self.opener = urllib.request.build_opener(urllib.request.HTTPCookieProcessor(self.cj))
        urllib.request.install_opener(self.opener)

    def get_csrf_token(self, url):
        try:
            req = urllib.request.Request(url)
            with urllib.request.urlopen(req) as resp:
                html = resp.read().decode('utf-8')
                token_match = re.search(r'name="__RequestVerificationToken" type="hidden" value="([^"]+)"', html)
                if not token_match:
                    token_match = re.search(r'value="([^"]+)" name="__RequestVerificationToken"', html)
                return token_match.group(1) if token_match else ""
        except Exception as e:
            safe_print(f"Error fetching CSRF token from {url}: {e}")
            return ""

    def login(self, username, password):
        login_url = f"{self.base_url}/Identity/Account/Login"
        token = self.get_csrf_token(login_url)
        
        login_data = {
            "Input.Identifier": username,
            "Input.Password": password,
            "Input.RememberMe": "false",
            "__RequestVerificationToken": token
        }
        encoded_data = urllib.parse.urlencode(login_data).encode('utf-8')
        
        try:
            req = urllib.request.Request(login_url, data=encoded_data, method='POST')
            req.add_header("Content-Type", "application/x-www-form-urlencoded")
            with urllib.request.urlopen(req) as resp:
                final_url = resp.geturl()
                return True, final_url
        except Exception as e:
            return False, str(e)

    def create_customer_order(self):
        # 1. Fetch CreateOrder page to get CSRF token
        create_url = f"{self.base_url}/Customer/CreateOrder"
        token = self.get_csrf_token(create_url)
        if not token:
            return False, "Failed to get CSRF token for order creation"

        # 2. Prepare order POST data
        order_data = {
            "TenNguoiNhan": "E2E Test Recipient",
            "SoDienThoaiNguoiNhan": "0987654321",
            "DiaChiNguoiNhan": "123 Test Street, Ward 5, Dist 1, HCMC",
            "TongKhoiLuong": "1.5",
            "HinhThucThanhToan": "COD",
            "__RequestVerificationToken": token
        }
        encoded_data = urllib.parse.urlencode(order_data).encode('utf-8')

        try:
            req = urllib.request.Request(create_url, data=encoded_data, method='POST')
            req.add_header("Content-Type", "application/x-www-form-urlencoded")
            with urllib.request.urlopen(req) as resp:
                final_url = resp.geturl()
                return True, final_url
        except Exception as e:
            return False, str(e)

    def logout(self):
        self.cj.clear()

    def test_page(self, relative_url, expected_contents):
        target_url = f"{self.base_url}{relative_url}"
        try:
            req = urllib.request.Request(target_url)
            with urllib.request.urlopen(req) as resp:
                status = resp.status
                final_url = resp.geturl()
                html = resp.read().decode('utf-8')
                
                # Check contents
                missing = []
                for exp in expected_contents:
                    if exp not in html:
                        missing.append(exp)
                        
                if missing:
                    return False, f"HTTP {status}, missing elements: {missing}", html
                return True, f"HTTP {status} (Redirected to: {final_url})", html
        except Exception as e:
            return False, f"Exception: {e}", ""

def run_tests():
    tester = SDMSTester()
    
    roles_tests = [
        {
            "role": "Admin",
            "username": "admin@sdms.com",
            "password": "123456",
            "landing_checks": ["Admin", "Trang Quản Trị Hệ Thống"],
            "endpoints": [
                ("/Admin/Staff", ["premium-table", "add-staff-modal"]),
                ("/Admin/Warehouses", ["premium-table", "add-warehouse-modal"]),
                ("/Admin/AuditLogs", ["premium-table", "glass"]),
                ("/Admin/OrderReport", ["OrderReport", "monthlyChart", "statusChart"])
            ]
        },
        {
            "role": "WarehouseStaff",
            "username": "warehouse@sdms.com",
            "password": "123456",
            "landing_checks": ["Trang Quản Trị Hệ Thống", "Thủ kho"],
            "endpoints": [
                ("/Warehouse/Inventory", ["premium-table", "glass"])
            ]
        },
        {
            "role": "Shipper",
            "username": "shipper@sdms.com",
            "password": "123456",
            "landing_checks": ["Trang Quản Trị Hệ Thống", "Nhân viên giao hàng"],
            "endpoints": [
                ("/Shipper/MyDeliveries", ["btn-start-scan", "scanner-hud", "laser-line"])
            ]
        },
        {
            "role": "Customer",
            "username": "customer@sdms.com",
            "password": "123456",
            "landing_checks": ["Thống kê & Báo cáo", "stat-total-orders", "pdf-report-printable-area"],
            "endpoints": [
                ("/Customer/Orders", ["Orders", "nav-links"]),
                ("/Customer/CreateOrder", ["order-container", "back-btn", "form-grid", "premium-card"])
            ]
        }
    ]

    safe_print("====================================================")
    safe_print("   SDMS ROLE-BASED END-TO-END AUTOMATED TEST RUNNER")
    safe_print("====================================================")
    
    total_checks = 0
    passed_checks = 0

    for test in roles_tests:
        safe_print(f"\n[ROLE] Testing role: {test['role']} ({test['username']})")
        
        # 1. Log in
        safe_print("  - Performing authentication...")
        success, info = tester.login(test["username"], test["password"])
        if not success:
            safe_print(f"    [FAIL] Authentication failed: {info}")
            continue
        safe_print(f"    [SUCCESS] Logged in. Landing redirect: {info}")
        
        # 2. Test root url role redirection
        safe_print("  - Testing root (/) redirection behavior...")
        total_checks += 1
        landing_ok, landing_info, _ = tester.test_page("/", test["landing_checks"])
        if landing_ok:
            safe_print(f"    [PASS] Root (/) redirected & rendered correctly: {landing_info}")
            passed_checks += 1
        else:
            safe_print(f"    [FAIL] Root (/) redirection failed: {landing_info}")
            
        # 3. Test other functional endpoints
        for endpoint, patterns in test["endpoints"]:
            safe_print(f"  - Visiting functional page: {endpoint}...")
            total_checks += 1
            ok, page_info, _ = tester.test_page(endpoint, patterns)
            if ok:
                safe_print(f"    [PASS] Page {endpoint} rendered correctly: {page_info}")
                passed_checks += 1
            else:
                safe_print(f"    [FAIL] Page {endpoint} check failed: {page_info}")

        # 4. If Customer, perform E2E Dynamic Order Creation & Order Tracking
        if test["role"] == "Customer":
            safe_print("  - Performing Dynamic Order Creation...")
            total_checks += 1
            create_ok, create_info = tester.create_customer_order()
            if create_ok:
                safe_print(f"    [PASS] Customer order dynamically created! Landing target: {create_info}")
                passed_checks += 1
                
                # Fetch order list to find the newly created order ID
                safe_print("  - Parsing newly created order ID from history...")
                total_checks += 1
                orders_ok, orders_info, orders_html = tester.test_page("/Customer/Orders", ["Orders"])
                if orders_ok:
                    # Search for DH followed by digits (e.g. DH63852...)
                    order_ids = re.findall(r'DH\d+', orders_html)
                    if order_ids:
                        new_order_id = order_ids[0]
                        safe_print(f"    [PASS] Successfully parsed new order ID: {new_order_id}")
                        passed_checks += 1
                        
                        # Verify Tracking page using the dynamic order ID
                        track_endpoint = f"/Customer/Track/{new_order_id}"
                        safe_print(f"  - Testing Dynamic Tracking page: {track_endpoint}...")
                        total_checks += 1
                        track_ok, track_info, _ = tester.test_page(track_endpoint, ["openPrintModal()", "Logistics"])
                        if track_ok:
                            safe_print(f"    [PASS] Dynamic Tracking page for {new_order_id} rendered perfectly: {track_info}")
                            passed_checks += 1
                        else:
                            safe_print(f"    [FAIL] Dynamic Tracking page for {new_order_id} check failed: {track_info}")
                    else:
                        safe_print("    [FAIL] No order ID found matching pattern DH\\d+ in history HTML!")
                else:
                    safe_print(f"    [FAIL] Accessing Orders page to retrieve ID failed: {orders_info}")
            else:
                safe_print(f"    [FAIL] Dynamic order creation failed: {create_info}")
                
        # 5. Log out
        tester.logout()
        safe_print(f"  - Session cleared for {test['role']}.")

    safe_print("\n====================================================")
    safe_print("                 TEST RUN SUMMARY")
    safe_print("====================================================")
    safe_print(f" Total checks executed: {total_checks}")
    safe_print(f" Total checks passed  : {passed_checks}")
    safe_print(f" Total checks failed  : {total_checks - passed_checks}")
    
    if passed_checks == total_checks:
        safe_print(" -> ALL SYSTEM FUNCTIONALITIES PASSED SUCCESSFULLY FOR EVERY ROLE!")
        sys.exit(0)
    else:
        safe_print(" -> SYSTEM RECORDED TEST FAILURES DURING END-TO-END VERIFICATION!")
        sys.exit(1)

if __name__ == "__main__":
    run_tests()
