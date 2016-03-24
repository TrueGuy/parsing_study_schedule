﻿using System;
using System.Net;
using System.Xml;

using System.Collections.Generic;

using HtmlAgilityPack;

namespace parsing_timetables
{

	public class Link {
		public string name, url;
		public Link(string _name, string _url){
			name = _name;
			url = _url;
		}
	}

	public class TimetableParser {

		private static HtmlDocument getHtmlFromUrl(string url){
			using (WebClient client = new WebClient ()){
				client.Headers.Add(HttpRequestHeader.Cookie, "_culture=ru"); 
				HtmlDocument html = new HtmlDocument();
				html.LoadHtml(client.DownloadString(url));
				return html;
			}		
		}

		private static HtmlNodeCollection findByClassContains(string className, HtmlNode node){
			return node.SelectNodes(string.Format("//*[contains(@class,'{0}')]", className));
		}

		private static HtmlNodeCollection findByIdContains(string id, HtmlNode node){

			return node.SelectNodes(string.Format("//*[contains(@id, '{0}')]", id))
				;
		}

		private static string getPlainText(HtmlNode node){
			var t = "";
			foreach(HtmlNode n in node.SelectNodes("text()")){
				t += n.InnerText;
			}
			return t;
		}

		public static List<string> getStudyLevels(){
			var res = new List<string> ();
			var html = getHtmlFromUrl("http://timetable.spbu.ru/AMCP");
			var studyLevelNodes = html.DocumentNode.SelectNodes ("//div[@id='accordion']/div/div[@class='panel-heading']/h4[contains(@class, 'panel-title')]/a[contains(@href, '#studyProgramLevel')]");
			if (studyLevelNodes != null) {
				foreach (var n in studyLevelNodes) {
					res.Add (getPlainText(n).Trim());
				}	
			}

			return res;
		}

	}
}

