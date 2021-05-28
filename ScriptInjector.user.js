// ==UserScript==
// @name         HFlashPlayer Script Injector
// @author       h-flash
// @version      0.1.1
// @include      *
// @grant        none
// @updateURL    https://raw.githubusercontent.com/h-flash/HFlashPlayer/main/ScriptInjector.user.js
// @downloadURL  https://raw.githubusercontent.com/h-flash/HFlashPlayer/main/ScriptInjector.user.js
// ==/UserScript==

(function (doc) {
	function tags(objs, embs) {
		var elems = [];
		for (var s = 0; s < objs.length; s++) {
			var obj = objs[s];
			if (obj.getElementsByTagName("EMBED").length > 0) continue;
			elems.push(obj);
		}

		for (var i = 0; i < embs.length; i++) {
			var emb = embs[i];
			elems.push(emb);
		}

		return elems;
	}

	function test(elem) {
		if (elem.getAttribute("type") && elem.getAttribute("type").toLowerCase() == "application/x-shockwave-flash") return true;
		if (elem.getAttribute("classid") && (elem.getAttribute("classid").toLowerCase() == "clsid:d27cdb6e-ae6d-11cf-96b8-444553540000")) return true;
		if (elem.data && elem.data.toLowerCase().indexOf(".swf") > -1) return true;
		if (objsrc(elem)) return true;
		return false;
	}

	function objsrc(elem) {
		if (elem.data && elem.data.toLowerCase().indexOf(".swf") > -1) return elem.data;
		var items = elem.getElementsByTagName("PARAM");
		for (var i = 0; i < items.length; i++) {
			var n = items[i].name;
			var v = items[i].value;
			if (n && n.toLowerCase() == "movie") return urlfix(v);
		}
		return null;
	}

	function urlfix(url) {
		if (url.toLowerCase().substr(0, 4) == "http") return url;
		var curl = window.location.href;
		if (url.substr(0, 1) == "/") {
			return curl.substr(0, curl.indexOf("/")) + url;
		} else {
			return curl.substr(0, curl.lastIndexOf("/") + 1) + url;
		}
	}

	function injector(doc) {
		var scheme = "hflash://";
		var target = "hflashplayer_target_frame";
		var frm = doc.getElementById(target);
		var nodes = tags(doc.getElementsByTagName("OBJECT"), doc.getElementsByTagName("EMBED"));
		var cnt = 0;
		for (var i = 0; i < nodes.length; i++) {
			var node = nodes[i];
			if (!test(node)) continue;

			var parent = node.parentNode;
			var rect = node.getBoundingClientRect();
			var left = rect.left;
			var top = rect.top;
			var swfpath = node.src || objsrc(node);
			var div = doc.createElement("DIV");
			var link = doc.createElement("A");
			var close = doc.createElement("A");

			if (parent.tagName == "OBJECT") {
				parent = parent.parentNode;
				node = node.parentNode;
			}

			link.href = scheme + btoa(swfpath);
			link.target = target;
			link.cssText = "color:#000;font-size:14px";
			link.innerHTML = 'Open by <span style="color:#FE719C">HFlashPlayer</span> ';
			close.innerHTML = " [X]";
			close.style.cursor = "pointer";
			close.onclick = function () {
				this.parentNode.parentNode.removeChild(this.parentNode)
			};

			div.style.cssText = "padding:6px 10px;background:#FFF;position:absolute;border:solid 2px #FF4DD2;border-radius:1px;left:" + left + "px";
			div.appendChild(link);
			div.appendChild(close);
			parent.insertBefore(div, node);
			cnt++;
		}


		if (cnt > 0 && !frm) {
			frm = doc.createElement("IFRAME");
			frm.name = frm.id = target;
			frm.style.display = "none";
			doc.body.appendChild(frm);
		}

		if (cnt > 0) console.log(cnt + " swf link converted.");
	}

	injector(doc);

})(document);

