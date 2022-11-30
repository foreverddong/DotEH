using DotEH.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DotEH.Model
{
    /*
     * Some Observations On Categories:
     * Misc -           1111111110
     * Doujinshi -      1111111101
     * Manga -          1111111011
     * Artist CG -      1111110111
     * Game CG -        1111101111
     * Image Set -      1111011111
     * Cosplay -        1110111111
     * Asian Porn -     1101111111
     * Non-H -          1011111111
     * Western -        0111111111
    */
    public class SearchParameter
    {
        public bool Misc
        {
            get
            {
                return !this.categoryCode.IsBitSet(0);
            }
            set
            {
                if (value)
                {
                    categoryCode &= ~(1u << 0); ;
                }
                else
                {
                    categoryCode |= 1 << 0;
                }
            }
        }
        public bool Doujinshi
        {
            get
            {
                return !this.categoryCode.IsBitSet(1);
            }
            set
            {
                if (value)
                {
                    categoryCode &= ~(1u << 1); ;
                }
                else
                {
                    categoryCode |= 1 << 1;
                }
            }
        }
        public bool Manga
        {
            get
            {
                return !this.categoryCode.IsBitSet(2);
            }
            set
            {
                if (value)
                {
                    categoryCode &= ~(1u << 2); ;
                }
                else
                {
                    categoryCode |= 1 << 2;
                }
            }
        }
        public bool ArtistCG
        {
            get
            {
                return !this.categoryCode.IsBitSet(3);
            }
            set
            {
                if (value)
                {
                    categoryCode &= ~(1u << 3); ;
                }
                else
                {
                    categoryCode |= 1 << 3;
                }
            }
        }
        public bool GameCG
        {
            get
            {
                return !this.categoryCode.IsBitSet(4);
            }
            set
            {
                if (value)
                {
                    categoryCode &= ~(1u << 4); ;
                }
                else
                {
                    categoryCode |= 1 << 4;
                }
            }
        }
        public bool ImageSet
        {
            get
            {
                return !this.categoryCode.IsBitSet(5);
            }
            set
            {
                if (value)
                {
                    categoryCode &= ~(1u << 5); ;
                }
                else
                {
                    categoryCode |= 1 << 5;
                }
            }
        }
        public bool Cosplay
        {
            get
            {
                return !this.categoryCode.IsBitSet(6);
            }
            set
            {
                if (value)
                {
                    categoryCode &= ~(1u << 6); ;
                }
                else
                {
                    categoryCode |= 1 << 6;
                }
            }
        }
        public bool AsianPorn
        {
            get
            {
                return !this.categoryCode.IsBitSet(7);
            }
            set
            {
                if (value)
                {
                    categoryCode &= ~(1u << 7); ;
                }
                else
                {
                    categoryCode |= 1 << 7;
                }
            }
        }
        public bool NonH
        {
            get
            {
                return !this.categoryCode.IsBitSet(8);
            }
            set
            {
                if (value)
                {
                    categoryCode &= ~(1u << 8); ;
                }
                else
                {
                    categoryCode |= 1 << 8;
                }
            }
        }
        public bool Western
        {
            get
            {
                return !this.categoryCode.IsBitSet(9);
            }
            set
            {
                if (value)
                {
                    categoryCode &= ~(1u << 9); ;
                }
                else
                {
                    categoryCode |= 1 << 9;
                }
            }
        }
        public uint categoryCode { get; set; } = 0;

        public List<string> Tags { get; set; } = new();

        public string Query = "";

        public SearchParameter(List<CategoryButtonState> categories, List<string> tags, string query)
        {
            this.Tags.AddRange(tags);
            this.Doujinshi = categories.IsCategorySet("Doujinshi");
            this.Manga = categories.IsCategorySet("Manga");
            this.ArtistCG = categories.IsCategorySet("Artist CG");
            this.GameCG = categories.IsCategorySet("Game CG");
            this.Western = categories.IsCategorySet("Western");
            this.NonH = categories.IsCategorySet("Non-H");
            this.ImageSet = categories.IsCategorySet("Image Set");
            this.Cosplay = categories.IsCategorySet("Cosplay");
            this.AsianPorn = categories.IsCategorySet("Asian Porn");
            this.Misc = categories.IsCategorySet("Misc");
            this.Query = query;
        }

    }

    public static class ShortExtensions
    {
        public static bool IsBitSet(this uint num, int index)
        {
            return (num & (1 << index)) != 0;
        }
    }

    public static class CategoryListExtensions
    {
        public static bool IsCategorySet(this List<CategoryButtonState> states, string name)
        {
            return states.First(s => s.Name == name).Enabled;
        }
    }
}
