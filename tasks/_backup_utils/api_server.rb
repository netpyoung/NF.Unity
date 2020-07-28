require 'hash_validator'
require 'rake'
require 'net/http'
require 'socket'
require 'json'
require 'semantic'
require 'digest/md5'

# TODO(pyoung): test version check server - using ruby ? python?
class APIServer
  attr_reader :api_server_url

  def initialize(api_server_url)
    @api_server_url = api_server_url
  end

  # bundle
  def GetCurrentBundleVersion(target)
    uri = URI("#{@api_server_url}/GetCurrentBundleVersion/")
    response = Net::HTTP.post_form(uri, {})
    unless response.kind_of? Net::HTTPSuccess
      raise "WTF #{uri} #{response}"
    end

	if (target == 'ANDROID')
      return (JSON.parse(response.body)["VersionInfo"][0]["Version"]).to_i
	else (target == 'IOS')
      return (JSON.parse(response.body)["VersionInfo"][1]["Version"]).to_i
	end
  end

  def UpdateBundleVersion(target, version)
    target_type = 0 if target == 'ANDROID'
	target_type = 1 if target == 'IOS'

    uri = URI("#{@api_server_url}/UpdateBundleVersion/")
    response = Net::HTTP.post_form(uri, {"Type" => target_type, "Version" => version, "HashData" => "dummydata"})
    unless response.kind_of? Net::HTTPSuccess
      raise "WTF #{uri} #{response}"
    end
    result = JSON.parse(response.body)
    if result["ReturnCode"] != 0
      raise "WTF #{uri} #{result}"
    end
  end

  # gamedb
  def GetCurrentGameDBVersion()
    uri = URI("#{@api_server_url}/GetCurrentGameDBVersion/")
    response = Net::HTTP.post_form(uri, {})
    unless response.kind_of? Net::HTTPSuccess
      raise "WTF #{uri} #{response}"
    end
    return (JSON.parse(response.body)["VersionInfo"][3]["Version"]).to_i
  end

  def UpdateGameDBVersion(version, hash)
    uri = URI("#{@api_server_url}/UpdateGameDBVersion/")
    response = Net::HTTP.post_form(uri, {"Type" => 3, "Version" => version, "HashData" => hash})
    unless response.kind_of? Net::HTTPSuccess
      raise "WTF #{uri} #{response}"
    end
    result = JSON.parse(response.body)
    if result["ReturnCode"] != 0
      raise "WTF #{uri} #{result}"
    end
  end

  # locale
  def GetCurrentLocaleVersion()
    uri = URI("#{@api_server_url}/GetCurrentLocaleVersion/")
    response = Net::HTTP.post_form(uri, {})
    unless response.kind_of? Net::HTTPSuccess
      raise "WTF #{uri} #{response}"
    end
    return (JSON.parse(response.body)["VersionInfo"][4]["Version"]).to_i
  end

  def UpdateLocaleVersion(version, hash)
    uri = URI("#{@api_server_url}/UpdateLocaleVersion/")
    response = Net::HTTP.post_form(uri, {"Type" => 4,"Version" => version, "HashData" => hash})
    unless response.kind_of? Net::HTTPSuccess
      raise "WTF #{uri} #{response}"
    end
    result = JSON.parse(response.body)
    if result["ReturnCode"] != 0
      raise "WTF #{uri} #{result}"
    end
  end


  # helper
  def with_gamedb(&block)
    block.call(0, 0)
    return [0, 0]
    pre_ver = self.GetCurrentGameDBVersion()
    nxt_ver = pre_ver + 1

    fpath = block.call(pre_ver, nxt_ver)

    self.UpdateGameDBVersion(nxt_ver.to_s, md5(fpath))
	return [pre_ver, nxt_ver]
  end

  def with_bundle(target, &block)
    block.call(0, 0)
    return [0, 0]

    pre_ver = self.GetCurrentBundleVersion(target)
    nxt_ver = pre_ver + 1

    block.call(pre_ver, nxt_ver)

    self.UpdateBundleVersion(target, nxt_ver.to_s)

	return [pre_ver, nxt_ver]
  end

  def with_locale(&block)
    block.call(0, 0)
    return [0, 0]
    pre_ver = self.GetCurrentLocaleVersion()
    nxt_ver = pre_ver + 1

    fpath = block.call(pre_ver, nxt_ver)

    self.UpdateLocaleVersion(nxt_ver.to_s, md5(fpath))

	return [pre_ver, nxt_ver]
  end

  def md5(fpath)
    Digest::MD5.file(fpath).hexdigest.upcase
  end
end
