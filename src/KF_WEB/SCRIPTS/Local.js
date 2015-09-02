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
function expandObject(targetID)
{
    return;
	var TableStyle = document.getElementById(targetID).style.display ;
	if ( TableStyle == "none" )
	{
		document.getElementById(targetID).style.display = "" ;
	}
	else
	{
		document.getElementById(targetID).style.display = "none" ;
	}
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
