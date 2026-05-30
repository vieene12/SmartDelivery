import urllib.request
import urllib.parse
import http.cookiejar
import re

def test_customer_flow():
    print("====================================================")
    print("   STARTING SDMS CUSTOMER AUTOMATED TEST PROGRAM")
    print("====================================================")

    # 1. Setup Cookie Jar to handle ASP.NET Core Identity cookies
    cj = http.cookiejar.CookieJar()
    opener = urllib.request.build_opener(urllib.request.HTTPCookieProcessor(cj))
    urllib.request.install_opener(opener)

    base_url = "http://localhost:5162"
    login_url = f"{base_url}/Identity/Account/Login"
    root_url = f"{base_url}/"

    # Step 1: Access Login Page to extract RequestVerificationToken
    print("\n[Step 1] Loading login page...")
    try:
        req = urllib.request.Request(login_url)
        with urllib.request.urlopen(req) as response:
            html = response.read().decode('utf-8')
            print(" -> Login page loaded successfully.")
    except Exception as e:
        print(f" -> ERROR: Cannot connect to server at {login_url}. Details: {e}")
        return

    # Regex to find ASP.NET Core anti-forgery token in input fields
    token_match = re.search(r'name="__RequestVerificationToken" type="hidden" value="([^"]+)"', html)
    if not token_match:
        token_match = re.search(r'value="([^"]+)" name="__RequestVerificationToken"', html)
        
    if not token_match:
        print(" -> WARNING: __RequestVerificationToken not found in login form.")
        token = ""
    else:
        token = token_match.group(1)
        print(f" -> Extracted CSRF token successfully: {token[:15]}...")

    # Step 2: Perform Login
    print("\n[Step 2] Performing authentication as Customer...")
    login_data = {
        "Input.Identifier": "customer@sdms.com",
        "Input.Password": "123456",
        "Input.RememberMe": "false",
        "__RequestVerificationToken": token
    }
    
    encoded_data = urllib.parse.urlencode(login_data).encode('utf-8')
    
    try:
        login_req = urllib.request.Request(login_url, data=encoded_data, method='POST')
        login_req.add_header("Content-Type", "application/x-www-form-urlencoded")
        
        with urllib.request.urlopen(login_req) as login_resp:
            final_url = login_resp.geturl()
            print(f" -> Authentication complete. Redirected to: {final_url}")
    except Exception as e:
        print(f" -> ERROR during authentication: {e}")
        return

    # Step 3: Access Root page to verify role-based landing redirection to Customer/Index
    print("\n[Step 3] Navigating to root (/) to verify role redirection...")
    try:
        req_root = urllib.request.Request(root_url)
        with urllib.request.urlopen(req_root) as root_resp:
            landing_url = root_resp.geturl()
            root_html = root_resp.read().decode('utf-8')
            print(f" -> Landing URL after visiting root (/): {landing_url}")
            if "/Customer" in landing_url or "/Customer/Index" in landing_url:
                print("  [PASS] Logged-in Customer is correctly redirected to /Customer dashboard!")
            else:
                print(f"  [FAIL] Redirect target is incorrect: {landing_url}")
    except Exception as e:
        print(f" -> ERROR navigating to root (/): {e}")
        return

    # Step 4: Verify Dashboard HTML components are properly rendered
    print("\n[Step 4] Verifying layout rendering of customer stats and PDF printable structures...")
    
    checks = {
        "Header title: Thong ke & Bao cao": "Thống kê & Báo cáo",
        "Metric Card: Total Orders (id=stat-total-orders)": 'id="stat-total-orders"',
        "Metric Card: Success Rate (id=stat-success-rate)": 'id="stat-success-rate"',
        "Metric Card: Total Shipping (id=stat-total-spent)": 'id="stat-total-spent"',
        "Metric Card: Total COD (id=stat-total-cod)": 'id="stat-total-cod"',
        "Filter select element (id=quick-date-filter)": 'id="quick-date-filter"',
        "Date input element: From Date (id=filter-from-date)": 'id="filter-from-date"',
        "Date input element: To Date (id=filter-to-date)": 'id="filter-to-date"',
        "Button: PDF Report Export (onclick=triggerReportPrint())": 'onclick="triggerReportPrint()"',
        "PDF Printable Area container (id=pdf-report-printable-area)": 'id="pdf-report-printable-area"'
    }

    all_passed = True
    for desc, pattern in checks.items():
        if pattern in root_html:
            print(f"  [PASS] {desc} is rendered properly.")
        else:
            print(f"  [FAIL] {desc} WAS NOT FOUND in HTML output!")
            all_passed = False

    print("\n====================================================")
    if all_passed:
        print("   CONCLUSION: ALL CUSTOMER FLOW TESTS PASSED SUCCESSFULLY!")
    else:
        print("   CONCLUSION: CUSTOMER FLOW TEST FAILURE DETECTED!")
    print("====================================================")

if __name__ == "__main__":
    test_customer_flow()
