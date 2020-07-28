import requests
import urllib3
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

BASE_URL = ""

def message(msg):
    print(msg)
    try:
        requests.post(BASE_URL,data={'message': msg})
    except Exception as e:
        print(e)
        pass
