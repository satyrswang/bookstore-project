using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore
{
    class Book
    {
        public string isbn;
        public string author;
        public string title;
        public int amount;
        public string publisher;
        public float cost_price;
        public float sell_price;
        public string language;
        public string type_id;
        public string shelf_id;
        public Book(string isbn1=null, string title1=null, string author1=null, string language1=null, int amount1=-1, string publisher1=null, float cost_price1=-1,
                    float sell_price1=-1, string shelf_id1=null, string type_id1=null)
        { 
            isbn = isbn1;
            author = author1;
            title = title1;
            amount = amount1;
            publisher = publisher1;
            cost_price = cost_price1;
            sell_price = sell_price1;
            language = language1;
            type_id = type_id1;
            shelf_id = shelf_id1;
        }

        //是否有属性为空
        public bool HasAnyNull()
        {
            return !(IsbnHasData() && TitleHasData() && AuthorHasData() && LanguageHasData() && AmountHasData()
                    && PublisherHasData() && Cost_priceHasData() && Sell_priceHasData() && Shelf_idHasData() &&
                    Type_idHasData());
        }

        //是否有属性有数据
        public bool HasAnyData()
        {
            return IsbnHasData() || TitleHasData() || AuthorHasData() || LanguageHasData() || AmountHasData()
                    || PublisherHasData() || Cost_priceHasData() || Sell_priceHasData() || Shelf_idHasData() ||
                    Type_idHasData();
        }

        public bool IsbnHasData() { return isbn != null && isbn != ""; }
        public bool TitleHasData() { return title != null && title != ""; }
        public bool AuthorHasData() { return author != null && author != ""; }
        public bool LanguageHasData() { return language != null && language != ""; }
        public bool AmountHasData() { return amount >= 0; }
        public bool PublisherHasData() { return publisher != null && publisher != ""; }
        public bool Cost_priceHasData() { return cost_price >= 0; }
        public bool Sell_priceHasData() { return sell_price >= 0; }
        public bool Shelf_idHasData() { return shelf_id != null && shelf_id != ""; }
        public bool Type_idHasData() { return type_id != null && type_id != ""; }


        public void Print()
        {
            Console.WriteLine(isbn+ " " + title + " " + author + " " + language + " " + amount + " "
                              + publisher + " " + cost_price + " " + sell_price + " " + shelf_id + " "
                              + type_id);
        }
    }
}
