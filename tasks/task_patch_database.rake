# -*- coding: utf-8 -*-
require "#{File.dirname(__FILE__)}/_os.rb"
require "#{File.dirname(__FILE__)}/_bot.rb"
require "#{File.dirname(__FILE__)}/_util.rb"

require 'json'
require 'digest/md5'

GAMEDATA_VERSION_FILENAME = "version.txt"
GAMEDATA_FTP_ROOT = "GAMEDATA"

def get_version_info(country, stage)
  FTP.get_ftp(country, stage) do |ftp|
    ftp.chdir(GAMEDATA_FTP_ROOT)
    ftp.getbinaryfile('version.txt', 'version.txt')
    ftp.chdir('..')
  end

  return JSON.parse(File.read('version.txt'))
end




########################################
#
# PATCH_DATABASE
#
########################################
desc "PATCH_DATABASE "
task :patch_database, [:COUNTRY, :STAGE] do |t, args|
  ## Validate
  validations = {
    :COUNTRY => ['KOR', 'JPN', 'ENG'],
    :STAGE => ['DEV']
  }
  validator = HashValidator.validate(args.to_hash, validations)
  if not validator.valid?
    bot("[DATAPATCH][FAIL] invalidate : #{args} | #{validations} | #{validator.errors}")
    exit -1;
  end

  ## args
  country, stage = args.COUNTRY, args.STAGE


  ## START



  ## Increase version number
  # sample: version.txt = {"version": {"1": "1"}, "latest": ["1", "md5"]}
  # some_text = File.open(text_fpath, "r:bom|utf-8").read
  version_json = get_version_info(country, stage)
  pre_ver = version_json['latest'][0].to_i
  nxt_ver = pre_ver + 1
  version_json['latest'] = [nxt_ver.to_s, Digest::MD5.file("#{GAMEDATA_ASSETBUNDLE_OUT_DIR}/gamedb").hexdigest.upcase];
  File.open("./#{GAMEDATA_VERSION_FILENAME}","w") do |file|
    file.write(version_json.to_json)
  end

  ## upload to FileServer
  FTP.get_ftp(country, stage) do |ftp|
    ftp.chdir(GAMEDATA_FTP_ROOT)
    ftp.mkdir(nxt_ver.to_s)
    ftp.putbinaryfile("#{GAMEDATA_ASSETBUNDLE_OUT_DIR}/gamedb", "#{nxt_ver}/gamedb")
    ftp.puttextfile("./#{GAMEDATA_VERSION_FILENAME}", "#{File.basename(GAMEDATA_VERSION_FILENAME)}")
  end

  ## END
  bot("[DATAPATCH][DONE] [#{country}_#{stage}]: #{pre_ver} => #{nxt_ver}")
end
