"use strict";

//json example
//let json = {
//    "status": "OK",
//    "rovers": [
//        {
//            "name": "rover_1",
//            "moves": [
//                {
//                    "x": "1",
//                    "y": "1",
//                    "o": "E"
//                },
//                {
//                    "x": "2",
//                    "y": "1",
//                    "o": "E"
//                },
//                {
//                    "x": "2",
//                    "y": "1",
//                    "o": "S"
//                },
//                {
//                    "x": "2",
//                    "y": "0",
//                    "o": "S"
//                },
//                {
//                    "x": "2",
//                    "y": "0",
//                    "o": "W"
//                },
//                {
//                    "x": "1",
//                    "y": "0",
//                    "o": "W"
//                },
//                {
//                    "x": "1",
//                    "y": "0",
//                    "o": "N"
//                }
//            ]
//        },
//        {
//            "name": "rover_2",
//            "moves": [
//                {
//                    "x": "4",
//                    "y": "1",
//                    "o": "E"
//                },
//                {
//                    "x": "5",
//                    "y": "1",
//                    "o": "E"
//                },
//                {
//                    "x": "5",
//                    "y": "1",
//                    "o": "S"
//                },
//                {
//                    "x": "5",
//                    "y": "0",
//                    "o": "S"
//                },
//                {
//                    "x": "5",
//                    "y": "0",
//                    "o": "W"
//                },
//                {
//                    "x": "4",
//                    "y": "0",
//                    "o": "W"
//                },
//                {
//                    "x": "4",
//                    "y": "0",
//                    "o": "N"
//                }
//            ]
//        }
//    ]
//};

let canvas, ctx;

function init(json) {
    
     canvas = document.getElementById("map");
     ctx = canvas.getContext("2d");
     
     //validate json returned data
    if (json != null) {
        console.log(json);
        showMoves(json);
    }
    else {

    }
}

// wrap setTimeout in a promise
const wait = async (ms = 1000) => new Promise((resolve) => setTimeout(resolve, ms));

async function showMoves(json) {
    const sizeX = 6;
    const sizeY = 6;
    const map = new PlateauMap(sizeX, sizeY);
    
    let data = json;

    // foreach rover
    for (let r = 0; r < data.rovers.length; r++) {
        // deploy rover
        let startPositionX = data.rovers[r].moves[0].x;
        let startPositionY = data.rovers[r].moves[0].y;
        let startPositionO = data.rovers[r].moves[0].o;
        let roverName = data.rovers[r].name;

        map.drawMap();
        map.drawDeployedRovers(r, data);
        map.drawRover(roverName, startPositionX, startPositionY, startPositionO, true);
        await wait(1000);

        // foreach rover's move, starting with the first move (move zero is the landing position)
        for (let m = 1; m < data.rovers[r].moves.length; m++) {
            let positionX = data.rovers[r].moves[m].x;
            let positionY = data.rovers[r].moves[m].y;
            let positionO = data.rovers[r].moves[m].o;

            map.drawMap();
            map.drawDeployedRovers(r, data);
            map.drawRover(roverName, positionX, positionY, positionO, true);
            await wait(1000);
        }
    }
}

function reportPosition(msg) {
    const paragraph = document.createElement("p");
    const logs = document.getElementById("log");
    const textNode = document.createTextNode(msg);

    paragraph.appendChild(textNode);
    // Append the text to <p>
    logs.appendChild(paragraph);
    logs.scrollTop = logs.scrollHeight;
}

class PlateauMap {
    constructor(sizeX, sizeY) {
        this.sizeX = sizeX;
        this.sizeY = sizeY;
        this.stepXLines = 100;
        this.stepYLines = 100;
        this.roverXOffset = 0;
        this.roverYOffset = (this.sizeY * this.stepYLines) - 100;
    }
    drawMap() {
        //update canvas size
        if (this.sizeX != 6 || this.sizeY != 6) {
            canvas.width = this.sizeX * this.stepXLines;
            canvas.height = this.sizeY * this.stepYLines;
        }

        let xLine = 0;
        let yLine = 0;

        let plateauImg = document.getElementById("plateau");
        ctx.drawImage(plateauImg, 0, 0, canvas.width, canvas.height);

        // set grid lines colour
        ctx.fillStyle = "black";

        // draw horizontal grid lines
        for (var x = 1; x < this.sizeX; x++) {
            xLine += this.stepXLines;
            ctx.fillRect(xLine, 0, 1, this.sizeY * this.stepYLines);
        }

        // draw vertical grid lines
        for (var y = 1; y < this.sizeY; y++) {
            yLine += this.stepYLines;
            ctx.fillRect(0, yLine, this.sizeX * this.stepXLines, 1);
        }

        // draw coordinates (0,0) and (5,5)
        ctx.font = "30px serif";
        ctx.fillStyle = "yellow";
        ctx.textBaseline = "middle";
        ctx.textAlign = 'center';
        ctx.fillText("(0,0)", 50, 550);
        ctx.fillText("(5,5)", 550, 50);
    }
    drawRover(roverName, x, y, orientation, logPosition) {
        if (logPosition === true) {
            reportPosition(`Current position for ${roverName}: [${x}, ${y}, ${orientation}]`);
        }

        const roverImg = document.getElementById("rover");

        const px = (this.stepXLines * x) + this.roverXOffset;
        const py = (this.stepYLines * y * -1) + this.roverYOffset; // by default the origin (0,0) is in the top left corner

        // save the current context  
        ctx.save();

        if (orientation == "N") {
            ctx.drawImage(roverImg, px, py, 100, 100);
        } else if (orientation == "E") {
            this.drawRotated(roverImg, px, py, 100, 100, 90);
        } else if (orientation == "S") {
            this.drawRotated(roverImg, px, py, 100, 100, 180);
        } else if (orientation == "W") {
            this.drawRotated(roverImg, px, py, 100, 100, -90);
        }

        // restore the context
        ctx.restore();
    }
    drawRotated(image, px, py, dx, dy, angle) {
        // translate to the center point of the image
        ctx.translate(px + (image.width * 0.5), py + (image.height * 0.5));
        // rotate  
        ctx.rotate((Math.PI / 180) * angle);
        // translate back to the top left of the image  
        ctx.translate(-(px + (image.width * 0.5)), -(py + (image.height * 0.5)));
        // draw the image  
        ctx.drawImage(image, px, py, dx, dy);
    }
    drawDeployedRovers(currentRoverIndex, data) {
        // draw last position of each already deployed rover
        for (let r = 0; r < currentRoverIndex; r++) {

            let lastMoveIndex = data.rovers[r].moves.length - 1;
            let lastPositionX = data.rovers[r].moves[lastMoveIndex].x;
            let lastPositionY = data.rovers[r].moves[lastMoveIndex].y;
            let lastPositionD = data.rovers[r].moves[lastMoveIndex].o;
            let roverName = data.rovers[r].name;

            this.drawRover(roverName, lastPositionX, lastPositionY, lastPositionD, false);
        }
    }
}

//TODO
//make an ajax calls instead
//add animation for the moves
//test translate(0,canvas.height); scale(1,-1); to get the well-known Cartesian coordinate system with the origin in the bottom left corner
//scale with negative numbers allows to do axis mirroring
//scale to increase or decrease the unit size (by default 1 unit = 1 pixel) - try set the scale to 100
//add favicon
//add text into resource
//add antiforgerytoken
//write unit tests
//improve html and styles
//add readme
