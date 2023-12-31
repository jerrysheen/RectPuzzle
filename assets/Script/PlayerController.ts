import { _decorator, Component, Vec3, systemEvent, SystemEvent, EventMouse, Animation, v3 } from 'cc';
const { ccclass, property } = _decorator;

@ccclass('PlayerController')
export class PlayerController extends Component {

    @property({ type: Animation })
    public BodyAnim: Animation = null;
    /* class member could be defined like this */
    // dummy = '';

    /* use `property` decorator if your want the member to be serializable */
    // @property
    // serializableDummy = 0;

    @property({ type: Number })
    public  _cubeId: number = 0;

    // for fake tween
    private _startJump: boolean = false;
    private _jumpStep: number = 0;
    private _curJumpTime: number = 0;
    private _jumpTime: number = 0.1;
    private _curJumpSpeed: number = 0;
    private _curPos: Vec3 = v3();
    private _deltaPos: Vec3 = v3(0, 0, 0);
    private _targetPos: Vec3 = v3();
    private _isMoving = false;

    @property({ type: Boolean})
    public Flipped : boolean  = false;

    start() {
        // Your initialization goes here.
        //systemEvent.on(SystemEvent.EventType.MOUSE_UP, this.onMouseUp, this);
    }

    //onMouseUp(event: EventMouse) {
    //    if (event.getButton() === 0) {
    //        this.jumpByStep(1);
    //    } else if (event.getButton() === 2) {
    //        this.jumpByStep(-2);
    //    }

    //}

    onClicked() {
        //this._startJump = true;
       // this._jumpStep = step;
        //this._curJumpTime = 0;
        //this._curJumpSpeed = this._jumpStep / this._jumpTime;
        //this.node.getPosition(this._curPos);
        //Vec3.add(this._targetPos, this._curPos, v3(this._jumpStep, 0, 0));

        this._isMoving = true;

        //if (step === 1) {
        //    this.BodyAnim.play('oneStep');
        //} else if (step === -2) {
        //    this.BodyAnim.play('twoStep');
        //}
        if(this.Flipped)
        {
            this.playSwitchOffAnim();
        }
        else
        {
            this.playSwitchOnAnim();
        }
        console.log(this._cubeId + "," + this.Flipped);
    }

    playSwitchOnAnim()
    {
        this.BodyAnim.play('SwitchOn');
        this.Flipped = true;
    }

    playSwitchOffAnim()
    {
        this.BodyAnim.play('SwitchOff');
        this.Flipped = false;
    }

    playerSwitchAnim()
    {
        if(this.Flipped)
        {
            this.playSwitchOffAnim();
        }
        else
        {
            this.playSwitchOnAnim();
        }
        console.log(this._cubeId + "," + this.Flipped);
    }

    onOnceJumpEnd() {
        this._isMoving = false;
    }

    update(deltaTime: number) {
        //if (this._startJump) {
        //    this._curJumpTime += deltaTime;
        //    if (this._curJumpTime > this._jumpTime) {
        //        // end
        //        this.node.setPosition(this._targetPos);
        //        this._startJump = false;
        //        this.onOnceJumpEnd();
        //    } else {
        //        // tween
        //        this.node.getPosition(this._curPos);
        //        this._deltaPos.x = this._curJumpSpeed * deltaTime;
        //        Vec3.add(this._curPos, this._curPos, this._deltaPos);
        //        this.node.setPosition(this._curPos);
        //    }
        //}
    }
}