from firebase_admin import credentials, initialize_app, storage
# Init firebase with your credentials
cred = credentials.Certificate("firebase-key.json")
initialize_app(cred, {'storageBucket': 'testing-d04b8.appspot.com'})

def upload_file(filepath,uploadpath):
    bucket = storage.bucket()
    blob = bucket.blob(uploadpath)
    blob.upload_from_filename(filepath)
    # Opt : if you want to make public access from the URL
    blob.make_public()
    print("your file url", blob.public_url)

def download_file(filepath):
    bucket = storage.bucket()
    blob = bucket.blob(filepath)
    blob.download_to_filename(filepath)