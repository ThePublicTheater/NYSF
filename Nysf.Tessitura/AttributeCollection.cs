using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    public class AttributeCollection : IEnumerable
    {
        public Attribute[] _attributes;

        public AttributeCollection(Attribute[] attributes)
        {
            _attributes = attributes;
        }

        public Attribute GetAttributeById(int id)
        {
            foreach (Attribute attribute in _attributes)
            {
                if (attribute.Id == id)
                    return attribute;
            }
            return null;
        }

        public Attribute GetAttributeByName(string name)
        {
            foreach (Attribute attribute in _attributes)
            {
                if (attribute.Name == name)
                    return attribute;
            }
            return null;
        }

        public Attribute this[int i]
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
            private Attribute[] _attributes;
            private int position = -1;

            public Enumerator(Attribute[] attributes)
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