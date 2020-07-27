namespace NFRuntime.Managements.OptionManagement
{
    public interface ISoundVolume
    {
        float VolumeMaster { get; set; }
        float VolumeBgm { get; set; }
        float VolumeEffect { get; set; }
        bool IsMute { get; set; }

        float GetVolumeBgm();
        float GetVolumeEffect();
    }
}