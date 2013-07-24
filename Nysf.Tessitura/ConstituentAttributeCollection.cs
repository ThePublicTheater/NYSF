using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    public class ConstituentAttributeCollection : IEnumerable
    {
        public ConstituentAttribute[] _attributes;

        public ConstituentAttributeCollection(ConstituentAttribute[] attributes)
        {
            _attributes = attributes;
        }

        public ConstituentAttribute[] GetAttributesById(int id)
        {
            List<ConstituentAttribute> found = new List<ConstituentAttribute>();
            foreach (ConstituentAttribute attribute in _attributes)
            {
                if (attribute.Id == id)
                    found.Add(attribute);
            }
            return found.ToArray();
        }

        public ConstituentAttribute[] GetAttributesByName(string name)
        {
            List<ConstituentAttribute> found = new List<ConstituentAttribute>();
            foreach (ConstituentAttribute attribute in _attributes)
            {
                if (attribute.Name == name)
                    found.Add(attribute);
            }
            return found.ToArray();
        }

        public int GetIdByName(string name)
        {
            foreach (ConstituentAttribute attribute in _attributes)
            {
                if (attribute.Name == name)
                    return attribute.Id;
            }
            return -1;
        }

        public Object[] GetValuesById(int id)
        {
            List<Object> values = new List<Object>();
            foreach (ConstituentAttribute attribute in GetAttributesById(id))
            {
                values.Add(attribute.Value);
            }
            return values.ToArray();
        }

        public bool HasAttributeValuePair(int attributeId, Object value)
        {
            foreach (ConstituentAttribute att in _attributes)
            {
                if (att.Id == attributeId && att.Value.Equals(value))
                    return true;
            }
            return false;
        }

        public ConstituentAttribute this[int i]
        {
            get { return _attributes[i]; }
        }

        public int Count
        {
            get { return _attributes.Length; }
        }

        public IEnumerator GetEnumerator()
        {
            return new Enumerator(_attributes);
        }

        private class Enumerator : IEnumerator
        {
            private ConstituentAttribute[] _attributes;
            private int position = -1;

            public Enumerator(ConstituentAttribute[] attributes)
            {
                _attributes = attributes;
            }

            public bool MoveNext()
            {
                position++;
                return position < _attributes.Length;
            }

            public void Reset()
            {
                position = -1;
            }

            public object Current
            {
                get
                {
                    try
                    {
                        return _attributes[position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }
    }
}