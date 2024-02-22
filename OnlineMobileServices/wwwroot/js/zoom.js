+function(){function ImageZoomService(){this._scaleBase=1.0
this._target=null
this._window=window
this._document=document
this._body=document.body
this._lastScrollPosition=null
this._handleClick=this._handleClick.bind(this)
this._handleKeyDown=this._handleKeyDown.bind(this)
this._handleScroll=this._handleScroll.bind(this)}
ImageZoomService.prototype={init:function(){this._body.addEventListener('click',this._handleClick)},_zoom:function(){this._target.zoomIn()
this._document.addEventListener('keydown',this._handleKeyDown)
this._document.addEventListener('scroll',this._handleScroll)},_close:function(){if(!this._target)return
this._target.zoomOut()
this._target=null
this._document.removeEventListener('keydown',this._handleKeyDown)
this._document.removeEventListener('scroll',this._handleScroll)},_handleClick:function(event){var target=event.target
if(!target)return
if(target.tagName==='IMG'&&target.hasAttribute('data-action')){switch(target.getAttribute('data-action')){case 'zoom':this._target=new Zoomable(target)
this._zoom()
break
case 'close':this._close()
break
default:break}}else{this._close()}},_handleKeyDown:function(event){if(event.keyCode===27)this._close()},_handleScroll:function(){var scrollTop=this._window.pageYOffset||(this._document.documentElement||this._body.parentNode||this._body).scrollTop
if(this._lastScrollPosition===null)this._lastScrollPosition=scrollTop
var deltaY=this._lastScrollPosition-scrollTop
if(Math.abs(deltaY)>=40){this._lastScrollPosition=null
this._close()}}}
function Zoomable(target){this._scaleBase=1.0
this._target=target
this._src=target.getAttribute('src')
this._original=target.getAttribute('data-original')
this._overlay=null
this._translate=null
this._scale=null
this._rect=this._target.getBoundingClientRect()
this._body=document.body
this._window=window
this._handleTransitionEnd=this._handleTransitionEnd.bind(this)
this._zoomOriginal=this._zoomOriginal.bind(this)}
Zoomable.prototype={zoomIn:function(){var img=new Image()
img.onload=(function(){if(this._original){this._target.style.width=img.width+'px'
this._target.style.height=img.height+'px'}
this._calculateZoom()
this._zoomOriginal()}).bind(this)
img.src=this._target.src},_zoomOriginal:function(){this._target.offsetWidth
this._target.setAttribute('data-action','close')
this._target.classList.add('image-zoom-img')
this._target.addEventListener('transitionend',this._handleTransitionEnd)
var transform='translate('+this._translate.x+'px,'+this._translate.y+'px) '+
'scale('+this._scale+','+this._scale+')'
setStyles(this._target,{'-webkit-transform':transform,'-ms-transform':transform,'transform':transform,})
this._overlay=document.createElement('div')
this._overlay.classList.add('image-zoom-overlay')
this._body.appendChild(this._overlay)
this._window.setTimeout((function(){this._body.classList.add('image-zoom-overlay-show')}).bind(this),50)},_calculateZoom:function(){var windowCenter={x:this._window.innerWidth/2,y:this._window.innerHeight/2}
var imgHalfWidth=this._rect.width/2
var imgHalfHeight=this._rect.height/2
var imgCenter={x:this._rect.left+imgHalfWidth,y:this._rect.top+imgHalfHeight}
this._translate={x:windowCenter.x-imgCenter.x,y:windowCenter.y-imgCenter.y}
var distFromImageEdgeToWindowEdge={x:windowCenter.x-imgHalfWidth,y:windowCenter.y-imgHalfHeight}
var scaleHorizontally=distFromImageEdgeToWindowEdge.x/imgHalfWidth
var scaleVertically=distFromImageEdgeToWindowEdge.y/imgHalfHeight
this._scale=this._scaleBase+Math.min(scaleHorizontally,scaleVertically)},zoomOut:function(){this._target.setAttribute('data-action','zoom')
this._target.addEventListener('transitionend',this._handleTransitionEnd)
this._body.classList.remove('image-zoom-overlay-show')
setStyles(this._target,{'-webkit-transform':'','-ms-transform':'','transform':'',})
if(this._original){this._target.setAttribute('src',this._src)}},_handleTransitionEnd:function(event){switch(this._target.getAttribute('data-action')){case 'zoom':this._body.removeChild(this._overlay)
this._target.classList.remove('image-zoom-img')
break
case 'close':if(this._original){this._target.setAttribute('src',this._original)}
break
default:break}
this._target.removeEventListener('transitionend',this._handleTransitionEnd)}}
function setStyles(element,styles){for(var prop in styles){element.style[prop]=styles[prop]}}
document.addEventListener('DOMContentLoaded',function(){new ImageZoomService().init()})}()