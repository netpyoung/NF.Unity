namespace NFRuntime.Managements.OptionManagement
{
    public interface ISoundVolume
    {
        float VolumeMaster { get; set; }
        float VolumeBgm { get; set; }
        float VolumeEffect { get; set; }

        // AudioSource.spatialBlend
        // effect : 1
        // other : 0
        // 0.0 makes the sound full 2D, 1.0 makes it full 3D.

        // TODO(pyoung): add VolumeSystem
        // TODO(pyoung): speed - audioSource.pitch
        // TODO(pyoung): add mute seperately
        bool IsMute { get; set; }

        float GetVolumeBgm();
        float GetVolumeEffect();
    }
}