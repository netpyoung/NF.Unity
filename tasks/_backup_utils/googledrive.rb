require 'google/apis/drive_v3'
require 'googleauth'
#OpenSSL::SSL::VERIFY_PEER = OpenSSL::SSL::VERIFY_NONE
# gem install google-api-client


module GoogleDrive
  def GoogleDrive.download_xlsx(sheet_id, project_json, dst_fpath)
    mime_type = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    scopes = ['https://www.googleapis.com/auth/drive.readonly']

    credentials = Google::Auth::ServiceAccountCredentials.make_creds(
      {:json_key_io => File.open(project_json),
       :scope => scopes
      }
    )

    drive = Google::Apis::DriveV3::DriveService.new
    drive.authorization = credentials
    drive.export_file(sheet_id, mime_type, download_dest: dst_fpath)
  end
end
