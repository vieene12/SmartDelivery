import os

file_path = r"c:\Users\Thanh Vy\OneDrive\Máy tính\DACS_UML\SDMS\SDMS\Views\Customer\CreateOrder.cshtml"

def safe_print(msg):
    try:
        print(msg)
    except UnicodeEncodeError:
        try:
            print(msg.encode('ascii', 'ignore').decode('ascii'))
        except Exception:
            pass

def search_keywords(keywords):
    if not os.path.exists(file_path):
        safe_print(f"File not found: {file_path}")
        return
        
    safe_print(f"Searching in {file_path}...")
    
    with open(file_path, 'r', encoding='utf-8') as f:
        lines = f.readlines()
        
    for kw in keywords:
        safe_print(f"\n--- Results for keyword: '{kw}' ---")
        matches = 0
        for i, line in enumerate(lines):
            if kw.lower() in line.lower():
                safe_print(f"Line {i+1}: {line.strip()}")
                matches += 1
                if matches > 50:
                    safe_print("... truncated after 50 matches ...")
                    break
        if matches == 0:
            safe_print(" -> No matches found.")

if __name__ == "__main__":
    search_keywords(["const addressData =", "onProvinceChange()", "updateCombinedAddress()"])
