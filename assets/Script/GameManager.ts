import { _decorator, Component, Node, PhysicsSystem, SystemEventType, systemEvent, geometry, Camera, Touch, EventTouch, Layers } from 'cc';
import { PlayerController } from './PlayerController';
const { ccclass, property } = _decorator;

@ccclass('GameManager')
export class GameManager extends Component {
	@property({ type: Camera, tooltip: '�����' })
	public mainCamera: Camera | null = null;

	@property({ type: Node, tooltip: '����������' })
	public node_touch_1: Node | null = null;

	private _ray: geometry.Ray = new geometry.Ray();

	start() {
		systemEvent.on(SystemEventType.TOUCH_START, this.onTouchStart, this);
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
						console.log('jump');
						player.jumpByStep(1);
					}
				}
			}
		}
	}
}
