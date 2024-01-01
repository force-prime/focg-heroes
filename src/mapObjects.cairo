use heroes::models::{MapObject, MapObjectDescription, UnitShopDescription, ResourceDescription, ArmyDescription};
use heroes::utils::{Vec2};
use heroes::player::Player;
use heroes::map::Map;
use starknet::ContractAddress;
use dojo::world::{IWorldDispatcher, IWorldDispatcherImpl};


#[generate_trait]
impl MapObjects of MapObjectTraits {
    fn buy_units(world: IWorldDispatcher, player: ContractAddress, obj: MapObject, count: u32) {
        let description = get!(world, (obj.description_id), (UnitShopDescription));

        assert(description.id != 0, 'UnitShopDescription missing');

        let totalCost = description.price * count;
        
        Player::subtract_gold(world, player, totalCost);
        Player::add_units(world, player, description.unit_id, count);
    }

    fn pickup_resource(world: IWorldDispatcher, player: ContractAddress, obj: MapObject) {
        let description = get!(world, (obj.description_id), (ResourceDescription));

        assert(description.id != 0, 'ResourceDescription missing');

        Player::add_gold(world, player, description.count);
        Map::remove_object(world, player, obj);
    }

    fn fight(world: IWorldDispatcher, player: ContractAddress, obj: MapObject) {
        let description = get!(world, (obj.description_id), (ArmyDescription));

        assert(description.id != 0, 'ArmyDescription missing');

        Map::remove_object(world, player, obj);
    }
} 