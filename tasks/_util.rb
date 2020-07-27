# -*- coding: utf-8 -*-
require 'file-tail'
require 'thread'


class String
  def to_path(end_slash=false)
    "#{'/' if self[0]=='\\'}#{self.split('\\').join('/')}#{'/' if end_slash}"
  end
end


# http://stackoverflow.com/questions/2108727/which-in-ruby-checking-if-program-exists-in-path-from-ruby
# Cross-platform way of finding an executable in the $PATH.
#
#   which('ruby') #=> /usr/bin/ruby
def which(cmd)
  exts = ENV['PATHEXT'] ? ENV['PATHEXT'].split(';') : ['']
  ENV['PATH'].split(File::PATH_SEPARATOR).each do |path|
    exts.each { |ext|
      exe = File.join(path, "#{cmd}#{ext}")
      return exe if File.executable?(exe) && !File.directory?(exe)
    }
  end
  return nil
end


def get_seconds(hour, minute)
  hour * 3600 + minute * 60
end


# http://flori.github.io/file-tail
class Logger

  def initialize(filename, interval)
    @thread = Thread.new do
      File.open(filename) do |log|
        log.extend(File::Tail)
        log.interval = interval
        log.backward(0)
        log.tail { |line| puts line }
      end
    end
  end

  def stop()
    sleep(1)
    @thread.exit
  end
end


def is_build_machine()
  return (ENV['BUILD_NUMBER'] != nil)
end


def strip_text (text)
  splited = text.split("\n")
  if splited.count == 1
    return text
  else
    return splited[1..-1].map(&:strip).join
  end
end
