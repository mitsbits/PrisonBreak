namespace PrisonBreak.Domain
{
    public interface IRobot
    {
        event RobotMovedEventHandler RobotMoved;

        PrisonBlock[] Escape(IPrison prison);
    }
}