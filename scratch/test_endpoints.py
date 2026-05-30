import urllib.request
import urllib.parse
import http.cookiejar
import re

def test_shipper_flow():
    print("====================================================")
    print("   STARTING SDMS AUTOMATED TEST PROGRAM")
    print("====================================================")

    # 1. Setup Cookie Jar to handle ASP.NET Core Identity authentication cookies
    cj = http.cookiejar.CookieJar()
    opener = urllib.request.build_opener(urllib.request.HTTPCookieProcessor(cj))
    urllib.request.install_opener(opener)

    base_url = "http://localhost:5162"
    login_url = f"{base_url}/Identity/Account/Login"
    deliveries_url = f"{base_url}/Shipper/MyDeliveries"

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
    print("\n[Step 2] Performing authentication as Shipper...")
    login_data = {
        "Input.Identifier": "shipper@sdms.com",
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

    # Step 3: Access Shipper Deliveries page
    print("\n[Step 3] Loading Shipper deliveries page...")
    try:
        with urllib.request.urlopen(deliveries_url) as deliv_resp:
            deliv_html = deliv_resp.read().decode('utf-8')
            status = deliv_resp.status
            print(f" -> Accessed MyDeliveries page successfully. HTTP response: {status} OK")
    except Exception as e:
        print(f" -> ERROR loading MyDeliveries page: {e}")
        return

    # Step 4: Verify Scanner HTML components are properly rendered
    print("\n[Step 4] Verifying layout rendering of new camera scanner components...")
    
    checks = {
        "Camera scan trigger button (btn-start-scan)": "btn-start-scan",
        "Camera reader element (id=\"reader\")": 'id="reader"',
        "Scanner HUD target (id=\"scanner-hud\")": 'id="scanner-hud"',
        "Glowing laser lines (id=\"laser-line\")": 'id="laser-line"',
        "Red error countdown overlays (id=\"scanner-countdown\")": 'id="scanner-countdown"',
        "Quick update status overlay (id=\"quick-update-modal\")": 'id="quick-update-modal"',
        "Quick update confirmation form (id=\"quick-confirm-form\")": 'id="quick-confirm-form"'
    }

    all_passed = True
    for desc, pattern in checks.items():
        if pattern in deliv_html:
            print(f"  [PASS] {desc} is rendered properly.")
        else:
            print(f"  [FAIL] {desc} WAS NOT FOUND in HTML output!")
            all_passed = False

    print("\n====================================================")
    if all_passed:
        print("   CONCLUSION: ALL TESTS PASSED SUCCESSFULLY!")
    else:
        print("   CONCLUSION: TEST FAILURE RECORDED IN RENDERING VERIFICATION!")
    print("====================================================")

if __name__ == "__main__":
    test_shipper_flow()
