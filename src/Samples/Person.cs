namespace Samples
{
    class Person
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Age { get; set; }
        public string Occupation { get; set; }

        public override string ToString()
        {
            return $"{LastName}, {FirstName}, {Age}, {Occupation}";
        }
    }
}
