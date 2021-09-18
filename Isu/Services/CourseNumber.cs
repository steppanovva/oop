namespace Isu.Services
{
    public class CourseNumber
    {
        private int _number;

        public int Number
        {
            get => _number;
            set
            {
                if (value > 0 && value < 7)
                    _number = value;
            }
        }
    }
}