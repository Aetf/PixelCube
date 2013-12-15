
namespace PixelCube.Interfaces
{
    /// <summary>
    /// 实现此接口以具有声音调整相关的功能
    /// </summary>
    public interface IVolume
    {
        /// <summary>
        /// 获取和设置是否静音
        /// </summary>
        bool Mute { get; set; }

        /// <summary>
        /// 获取和设置音量。
        /// 范围0~100
        /// </summary>
        //double VolumeLevel;
    }
}
