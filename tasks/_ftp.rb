# -*- coding: utf-8 -*-
require 'net/ftp'

class FTP
  def self.get_ftp(country, stage)
    ftp = get(country, stage)
    begin
      yield ftp
    ensure
      ftp.close
    end
  end

  def self.get(country, stage)
    case country
    when 'JPN'
      ftp = Net::FTP.new("1.1.1.1", "ftp", "password")
      ftp.chdir('_____JPN')
      ftp.chdir(stage)
      return ftp
    end
  end
end
