public interface ISoundHandler
{
    public void PlaySFXSound(SFX sfx);
    public void PlayAmbienceSound(Ambience ambience);
    public void PlayBGMusic();
    public void MuteAll();
    public void UnMuteAll();
    public void MuteBgMusic();
    public void MuteAmbience();
}
