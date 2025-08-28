using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMakerLib.MatchMakerModel
{
    public abstract class MatchMakerElement : IEquatable<MatchMakerElement>
    {
        public enum ElementStatus { READY, RUNNING, FINISHED,INACTIVE};

        public int Id { get; set; }
        public string Name { get; set; } = "";
        public ElementStatus State { get; set; } = ElementStatus.READY;

        public MatchMakerElement()
        {
        }
        public MatchMakerElement(string name) { Name = name; }
        public override string ToString()
        {
            return Name;
        }
        public override int GetHashCode()
        {
            return Id ;
        }
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            MatchMakerElement? objAsMatchMakerElement = obj as MatchMakerElement;
            if (objAsMatchMakerElement == null) return false;
            else return Equals(objAsMatchMakerElement);

        }
        public bool Equals(MatchMakerElement? other)
        {
            if (other != null)
            {
                if (other.Id == Id)
                {
                    return true;
                }
             }
            return false;
        }

    }
}
