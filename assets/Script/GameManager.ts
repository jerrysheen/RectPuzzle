import { _decorator, Component, Node, PhysicsSystem, SystemEventType, systemEvent, geometry, Camera, Touch, EventTouch, Layers, Prefab, instantiate, Vec3, Mesh } from 'cc';
import { PlayerController } from './PlayerController';
const { ccclass, property } = _decorator;

@ccclass('GameManager')
export class GameManager extends Component {
	@property({ type: Camera, tooltip: 'Camera' })
	public mainCamera: Camera | null = null;

	@property({ type: Prefab, tooltip: 'RedPrefab' })
	public redPrefab: Prefab | null = null;

	@property({ type: Node, tooltip: 'CubeRoot' })
	public cubeRoot: Node | null = null;

	@property({ type: Number, tooltip: 'Cube Size' })
	public cubeSize: number = 2;

	@property({ type: Number, tooltip: 'padding' })
	public padding: number = 0.5;


	private _ray: geometry.Ray = new geometry.Ray();
	private cubeList : Map<number, Node> = new Map<number, Node>();

	private mapData : string[][] = [['1','2','1'],
	['1','2','1'], 
	['2','2','1']];

	start() {
		systemEvent.on(SystemEventType.TOUCH_START, this.onTouchStart, this);

		// mapData[0][0] is at 0,0,0 position
		// 2 means switch on

		for (let i = 0; i < this.mapData.length; i++) {
			const element = this.mapData[i];
			for (let j = 0; j < element.length; j++) {
				const element2 = element[j];
				if(element2 === '0') continue;
				let redCube = instantiate(this.redPrefab);
				redCube?.setParent(this.cubeRoot);
				redCube?.setWorldPosition(new Vec3(j * this.cubeSize + this.padding * (j - 1), 0 , i * this.cubeSize + this.padding * (i - 1) ));
				let playerControllerScript = redCube?.getComponent(PlayerController);
				if(!playerControllerScript._cubeId)
				{
					console.log("set cube ID");
					playerControllerScript._cubeId = i * element.length + j;
					this.cubeList.set(playerControllerScript._cubeId, redCube);
				}

				if (element2 === '1')
				{
					playerControllerScript.playSwitchOffAnim();
				}
				else if(element2 === '2')
				{
					playerControllerScript.playSwitchOnAnim();
				}

			}
		}

	}

	changeNeighBorState(cubeId: number)
	{
		let cubeNode = this.cubeList.get(cubeId);
		if(cubeNode)
		{
			let playerControllerScript = cubeNode.getComponent(PlayerController);
			if(playerControllerScript)
			{
				let height = Math.floor(cubeId  / this.mapData[0].length);
				let width = cubeId - this.mapData[0].length * height;
				let tempArr = [-1, 0,1,0,-1];
				console.log("IndexX: " + width + ", IndexY: " + height);
				for(let i = 0; i < 4; i++)
				{
					let currIndexX = width + tempArr[i];
					let currIndexY = height + tempArr[i + 1];
					console.log("currIndexX: " + currIndexX + ", currIndexY: " + currIndexY);

					if(currIndexX < 0 || currIndexX >= this.mapData[0].length || currIndexY < 0 || currIndexY >= this.mapData.length || this.mapData[currIndexX][currIndexY] === '0') continue;
					let currCubeId = currIndexX+ currIndexY * this.mapData.length ;
					let currCubeNode = this.cubeList.get(currCubeId);
					if(currCubeNode)
					{
						let currPlayerControllerScript = currCubeNode.getComponent(PlayerController);
						if(currPlayerControllerScript)
						{
							currPlayerControllerScript.playerSwitchAnim();
						}
					}
				}
			}
		}

	}


	onTouchStart(touch: Touch, event: EventTouch) {
		// ��������� ������
		this.mainCamera?.screenPointToRay(event.getLocation().x, event.getLocation().y, this._ray);
		// ����������ײ�������߼��
		if (PhysicsSystem.instance.raycast(this._ray)) {
			console.log('Current Click');
			const r = PhysicsSystem.instance.raycastResults;
			for (let index = 0; index < r.length; index++) {
				const element = r[index];
				let currNode = element.collider.node;
				// 0 means cube Layer
				if (currNode.layer === 1 << 0)
				{
					let player = currNode.parent.getComponent(PlayerController);
					if (player)
					{
						player.onClicked();
						// stop when first cube is clicked
						this.changeNeighBorState(player._cubeId);
						break;
					}
				}
			}
		}
	}
}
