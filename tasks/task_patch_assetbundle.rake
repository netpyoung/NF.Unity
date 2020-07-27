# -*- coding: utf-8 -*-
require "#{File.dirname(__FILE__)}/_os.rb"
require "#{File.dirname(__FILE__)}/_bot.rb"
require "#{File.dirname(__FILE__)}/_util.rb"

require 'rake'
require 'hash_validator'


# require 'oga' # for xml



GAMEDATA_ASSETBUNDLE_DIR = "#{ROOT_DIR}/A5Unity/A5"
GAMEDATA_ASSETBUNDLE_OUT_DIR = "#{GAMEDATA_ASSETBUNDLE_DIR}/Assets/patch/output"
GAMEDATA_ASSETBUNDLE_OUTBIN_DIR = "#{GAMEDATA_ASSETBUNDLE_DIR}/Assets/patch/output_xml"
GAMEDATA_ASSETBUNDLE_INPUT_DIR = "#{GAMEDATA_ASSETBUNDLE_DIR}/Assets/patch/input"



########################################
#
# PATCH_ASSETBUNDLE
#
########################################

desc "PATCH_ASSETBUNDLE"
task :patch_assetbundle, [:COUNTRY, :STAGE, :TARGET] do |t, args|
  ## Validation
  validations = {
    :COUNTRY => ['KOR', 'JPN', 'ENG'],
    :STAGE => ['DEV'],
    :TARGET => ['ADRENO', 'MALI', 'IOS'],
  }

  validator = HashValidator.validate(args.to_hash, validations)
  if not validator.valid?
    bot("[DATAPATCH][FAIL] invalidate : #{args} | #{validations} | #{validator.errors}")
    exit -1;
  end

  # ref: http://blog.naver.com/3takugarden/220397979971
  # Adreno, Mali, Tegra


  # Power VR | RGB PVRTC 4bit | RGBA PVRTC 4bit
  # Adreno   | RGB ATC 4bit   | RGBA ATC 8bit
  # Mali-ARM | RGB ETC 4bit   | RGBA 32bit
  # Mali-T624| TGB ASTC 6*6   | RGBA ASTC 4x4
  # Tegra    | DXT1           | DXT5


  # Systeminfo.deviceModel
  # UnityEngine.iOS.Device.generation


  ## args
  country, stage, target = args.COUNTRY, args.STAGE, args.TARGET

  ## Start
  FileUtils.touch(UNITY_LOG_FPATH)
  logger = Logger.new(UNITY_LOG_FPATH, 1) do |line|
    puts line
  end
  cmd_run_unity = <<EOS.gsub(/\s+/, " ").strip
#{UNITY}
    -quit
    -batchmode
    -projectPath    #{UNITY_PRJ_ROOT}
    -logFile        #{LOG_FILE}
    -executeMethod  nsEdit.Patcher.ExecuteMethods.BuildAssetBundle
    -CustomArgs:COUNTRY=#{country};STAGE=#{stage};TARGET=#{target}
EOS

  if OS.mac?
    cmd_run_unity.gsub! ';', '\;'
  end

  sh cmd_run_unity do |ok, res|
    logger.stop()
    if not ok
      exit -1
    end
  end

  ## end
end
