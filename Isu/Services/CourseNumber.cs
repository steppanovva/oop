namespace Isu.Services
{
    public class CourseNumber
    {
        private int _number;

        public int Get()
        {
            return _number;
        }

        public void Set(int x)
        {
            if (x > 0 && x < 7)
                _number = x;
        }
    }
}