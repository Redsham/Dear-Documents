using UnityEngine;

namespace Content.Person.Documents
{
    public readonly struct SerialNumber
    {
        private SerialNumber(string value) => Value = value;
        public string Value { get; }
        
        
        public static SerialNumber Generate() => new($"{Random.Range(0, 10000):0000} {Random.Range(0, 1000000):000000}");
        public SerialNumber GetWrong()
        {
            SerialNumber result;
            
            do { result = Generate(); } 
            while (result == this);

            return result;
        }
        
        
        public static implicit operator string(SerialNumber serialNumber) => serialNumber.Value;
        
        public static bool operator ==(SerialNumber a, SerialNumber b) => a.Value == b.Value;
        public static bool operator !=(SerialNumber a, SerialNumber b) => a.Value != b.Value;
        
        public override bool Equals(object obj) => obj is SerialNumber other && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();
    }
}