use heroes::models::{MapObject, MapObjectDescription, PositionToCellData, MapObjectData};
use heroes::utils::{Vec2, Vec2Trait};

use dojo::world::{IWorldDispatcher, IWorldDispatcherImpl};
use starknet::{ContractAddress};

#[generate_trait]
impl Map of MapTraits {
    fn get_object_at_pos(world: IWorldDispatcher, player: ContractAddress, position: Vec2) -> Option<MapObject> {
        let posToMapObject = get!(world, (position), (PositionToCellData));

        if posToMapObject.object_id == 0 {
            return Option::None;
        }

        let obj = get!(world, (player, posToMapObject.object_id), (MapObject));

        // TODO: use better way
        if obj.position.is_zero() {
            let baseObj = get!(world, (posToMapObject.object_id), (MapObjectData));
            assert(baseObj.id != 0, 'base object not found');

            return Option::Some(MapObject {
                player: player,
                id: posToMapObject.object_id,
                position: baseObj.position,
                description_id: baseObj.description_id
            });
        }

        if obj.description_id == 0 {
            return Option::None;
        }

        return Option::Some(obj);
    }


    fn remove_object(world: IWorldDispatcher, player: ContractAddress, obj: MapObject) {
        let mut copy = obj;
        copy.description_id = 0;
        set!(world, (copy));
    }

    fn is_valid_map_position(world: IWorldDispatcher, position: Vec2) -> bool {
        return true;
    }

    fn is_walkable_position(world: IWorldDispatcher, position: Vec2) -> bool {
        return true;
    }

    fn is_adjacent_position(position1: Vec2, position2: Vec2) -> bool {
        return true;
    }
} 