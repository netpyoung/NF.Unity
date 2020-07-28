module Global
  require 'ostruct'
  require 'yaml'

  def self.config
    @config ||= to_ostruct(YAML.load_file("config.yaml"))
  end

  def self.to_ostruct(object)
    case object
    when Hash
      OpenStruct.new(Hash[object.map {|k, v| [k, to_ostruct(v)] }])
    when Array
      object.map {|x| to_ostruct(x) }
    else
      object
    end
  end
end
