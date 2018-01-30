using System;
using System.Collections.Generic;
using System.Text;

namespace DrosDbCreator
{
    public class wp_posts
    {
        public int ID { get; set; }
        public int post_author { get; set; }
        public int post_parent { get; set; }
        public int menu_order { get; set; }
        public int comment_count { get; set; }
        public string post_date { get; set; }
        public string post_date_gmt { get; set; }
        public string post_content { get; set; }
        public string post_title { get; set; }
        public string post_excerpt { get; set; }
        public string post_status { get; set; }
        public string comment_status { get; set; }
        public string ping_status { get; set; }
        public string post_password { get; set; }
        public string post_name { get; set; }
        public string to_ping { get; set; }
        public string pinged { get; set; }
        public string post_modified { get; set; }
        public string post_modified_gmt { get; set; }
        public string post_content_filtered { get; set; }
        public string guid { get; set; }
        public string post_type { get; set; }
        public string post_mime_type { get; set; }
    }

    public class wp_term_relationships
    {
        public int object_id { get; set; }
        public int term_taxonomy_id { get; set; }
        public int term_order { get; set; }
    }

    public class wp_term_taxonomy
    {
        public int term_taxonomy_id { get; set; }
        public int term_id { get; set; }
        public int parent { get; set; }
        public int count { get; set; }
        public string taxonomy { get; set; }
        public string description { get; set; }
    }

    public class wp_terms
    {
        public string term_id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string term_group { get; set; }
    }
}
