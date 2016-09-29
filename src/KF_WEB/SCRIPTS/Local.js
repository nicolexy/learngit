 var currrotation = 4;

function ShowDetailImg(divid,imgid,imgsrc)
{
	imgid.src=imgsrc.src;
	
	
	divid.style.left = 200;
	divid.style.top = 100;
	//divid.style.left=event.x+document.body.scrollLeft + 20;
	//divid.style.top=event.y+document.body.scrollTop;
	
	divid.style.visibility = "visible"; 
} 

function ShowMoveImg(divid,imgid,imgsrc,hiddendiv)
{
	imgid.src=imgsrc.src;
	
	divid.style.left = 200;
	divid.style.top = 100;
	
	//divid.style.left=event.x+document.body.scrollLeft + 20;
	//divid.style.top=event.y+document.body.scrollTop;
	
	divid.style.visibility = "visible"; 
	HiddenImg(hiddendiv);
} 
 
function ShowImg(divid,imgid,imgsrc,hiddendiv)
{
    currrotation =4;
    
    imgid.src=imgsrc.src;
	imgid.style.filter="progid:DXImageTransform.Microsoft.BasicImage( Rotation="+currrotation+")";
	
    imgid.width = 400;
    imgid.height = 300;
	
	
	//divid.style.height="200px";
	//divid.style.left=event.x+document.body.scrollLeft + 20;
	//divid.style.top=event.y+document.body.scrollTop;
	
	//divid.style.width="200px";
	divid.style.left = 200;
	divid.style.top = 100;
	
	divid.style.visibility = "visible"; 
	HiddenImg(hiddendiv);
}


function HiddenImg(divid)
{
	divid.style.visibility = "hidden"; 
}

 function ZoomOut(img)
{
	img.width = img.width*2;
	img.height = img.height*2;
}
		
function ZoomIn(img)
{
	img.width = img.width/2;
	img.height = img.height/2;
}


function RotaRight(img)
{
    currrotation ++;
	if(currrotation > 4)
	{
		currrotation = 1;
	}

	img.style.filter="progid:DXImageTransform.Microsoft.BasicImage( Rotation="+currrotation+")";	
}

function RotaLeft(img)
{
	currrotation --;
	
	if(currrotation < 1)
	{ 
	currrotation =4;
	}
	
	img.style.filter="progid:DXImageTransform.Microsoft.BasicImage( Rotation="+currrotation+")";	 
}
		  


// Expand Appointed Object
function expandObject(target)
{
    var getChildNodes=function(ele){
        var childArr = ele.children || ele.childNodes,  //shit IE
            childArrTem = new Array();
        for (var i = 0; i < childArr.length; i++) {
            if(childArr[i].nodeType==1)
            {
                childArrTem.push(childArr[i]);
            }
        }
        return childArrTem;
    }

    var event = target.event || window.event;
    var tar = event.target || event.srcElement;
    if (tar.nodeName.toLowerCase() == "a") return;

    var curch=target.children[1];
    var display = curch.style.display == "" ? "none" : "";
    var sbiling = getChildNodes(target.parentElement.parentElement);
    for (var i = 0; i < sbiling.length; i++) {
        try {
            var span = getChildNodes(sbiling[i]);
            var menuchild = getChildNodes(span[0])[1];
            menuchild.style.display = "none";
        } catch (e) {}
    }
    curch.style.display = display;
}

// Change DataGrid Class By CheckBox
function CheckBoxChange(source, oriClassName, selectClassName)
{
	if ( source.checked == true )
	{
		source.parentElement.parentElement.className = selectClassName ;
	}
	else
	{
		source.parentElement.parentElement.className = oriClassName;
	}
}

// Change CheckBox on Every Row in DataGrid By Header CheckBox
function GridControlChange( source )
{
	var sourceID = source.id;

	var frontIndex  = sourceID.indexOf( "__ctl" ) + 5;
	var frontString = sourceID.substring(0, frontIndex);

	var bString   = sourceID.substring(frontIndex);
	var backIndex = bString.indexOf("_");

	var vID = bString.substring(0, backIndex );
	var backString = bString.substring(backIndex);

	var vType;
	var targetID;

	while ( true )
	{
		vID ++ ;
		targetID = frontString + vID + backString;

		if ( document.getElementById(targetID) == null )
		{
			break;
		}

		vType = document.getElementById(targetID).type.toLowerCase();
		if ( vType == "checkbox" )
		{
			document.getElementById(targetID).checked = source.checked;
			document.getElementById(targetID).fireEvent("onclick");
		}
	}
}
