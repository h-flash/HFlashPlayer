# HFlashPlayer

A handler for automatically launch offline Flash player from a custom scheme url


## How to use:

* this tool is only deployed on __!nsfw!__ https://h-flash.com currently.

1. download this tool from release.
2. extract it to any location you want.
3. run HFlashPlayer.exe one time, accept user admin control pop out to update registry.
4. open a flash page on h-flash, click the "CLICK TO PLAY" link.

## How to use on all sites:

* we need a user script to add buttons on flash plugin objects.

1. install user script browser extension Tampermonkey.  
   chrome: https://chrome.google.com/webstore/detail/tampermonkey/dhdgffkkebhmkfjojejmpbldmpobfkfo  
   firefox: https://addons.mozilla.org/en-US/firefox/addon/tampermonkey/   
2. click Tampermonkey icon -> Dashboard -> Utilities -> Install from URL, copy and paste this url to install: https://raw.githubusercontent.com/h-flash/HFlashPlayer/main/ScriptInjector.user.js

## How to use for webmaster:

1. (optional) Edit Scheme property in Form1.cs for your custom scheme, then rebuild.
2. Combine scheme header with base64 encoded swf full path  as a launch url.
example: var url = "hflash://" + base64encode("http://yoursite.com/"+swfpath);

hints for webmaster:
* swfpath need not urlencode.
* modern browsers has built in base64encode function, just: btoa(swfpath) .
* if you open the launch url in a new window, it will leave a blank window, if not, the web page will stop functioning after launch. h-flash's solution: open in a hidden iframe.

## Issue self check list:
<pre>
When you click a "CLICK TO PLAY" or "HFlashPlayer" button:
Q1. nothing happened.
	A11. your HFlashPlayer was not registered correctly. please run HFlashPlayer.exe to register.
		Q111. nothing happens when I run HFlashPlayer.exe
			A111. you need .net framework 4.0 to run this program.
	A12. your web browser or antivirus blocked the request, check your browser's application setting or try in another web browser.

Q2. a blank window and it's keeps blank forever.
	A21. click file menu, you will see a url on position 1, carefully type and open it in your web browser to test the file is reachable.
		Q211. I saw a browser check or a captcha, or I saw the check a while ago.
			A211: your network was marked as unsafe by the firewall, you need to download and play offline, or use a proxy.
		Q212. I can download the file but I'm using a proxy.
			A212: you need to proxy the flashplayer.exe also.                                   
</pre>

if your situation not in the list please submit a new issue.
