namespace Web.Services
{
    public static class UkrainianPlural
    {
        /// <summary>Слово «товар» у потрібній формі: 1 товар, 2-4 товари, 5-20 товарів, 21 товар...</summary>
        public static string Goods(int count)
        {
            var lastTwo = count % 100;
            var last = count % 10;

            if (lastTwo is >= 11 and <= 14) return "товарів";
            return last switch
            {
                1 => "товар",
                >= 2 and <= 4 => "товари",
                _ => "товарів"
            };
        }
    }
}
