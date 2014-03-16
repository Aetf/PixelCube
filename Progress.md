##活动定义与进展##

1. 3D
    1. 小方块 #1
        1. Init
        2. Color
        3. Position
        4. Visible
        5. Hasfocus
        6. Serialization
    2. 背景 #2
        1. Init
        2. Fill
    3. 显示容器 #3
        1. Init
        2. Draw
        3. Matrix (Zoom, pan/rotate)
2. 音效
    1. 效果音 #4
        1. Handle event from 3.2
        2. Enable
        3. 独立thread
    2. 背景音 #5
        1. Enable
        2. 独立播放
3. UI
    1. Leapmotion封装 #6
        1. 定义手势
        2. 手势检测 (Event source)
        3. 设备init
    2. 操作实现 #7
        1. Handle event from #6
        2. 实际数据操作
        3. 操作通知 (Event source)
4. 序列化
    1. 自动保存 #8
    2. 导入 #9
    3. 导出 #10
5. 总体框架
    1. UI框架 #11
    2. 生命周期管理 (Event source) #12
