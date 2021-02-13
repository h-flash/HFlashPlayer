# HFlashPlayer

A handler for automatically launch offline Flash player from a custom scheme url


## How to use:

* this tool is only deployed on __!nsfw!__ https://h-flash.com currently.

1. download this tool from release.
2. extract it to any location you want.
3. run HFlashPlayer.exe one time, accept user admin control pop out to update registry.
4. open a flash page on h-flash, click the "CLICK TO PLAY" link.


## How to use for webmaster:

1. (optional) Edit Scheme property in Form1.cs for your custom scheme, then rebuild.
2. Combine scheme header with base64 encoded swf full path  as a launch url.
example: var url = "hflash://" + base64encode("http://yoursite.com/"+swfpath);

hints for webmaster:
* swfpath need not urlencode.
* modern browsers has built in base64encode function, just: btoa(swfpath) .
* if you open the launch url in a new window, it will leave a blank window, if not, the web page will stop functioning after launch. h-flash's solution: open in a hidden iframe.
