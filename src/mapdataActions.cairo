use heroes::utils::{Direction};
use heroes::utils::{Vec2};
use heroes::models::{PositionToCellData, PlayerPosition, ResourceDescription, UnitShopDescription, ArmyDescription, MapObjectData};

#[starknet::interface]
trait IMapdataActions<TContractState> {
    fn upload_map(self: @TContractState, cells: Array<PositionToCellData>, mapObjects: Array<MapObjectData>, startPosition: Vec2);
    fn upload_descriptions(self: @TContractState, resources: Array<ResourceDescription>, unitShops: Array<UnitShopDescription>, armies: Array<ArmyDescription>);
}

#[dojo::contract]
mod mapdataActions {
    use starknet::{ContractAddress, get_caller_address};
    use heroes::models::{PlayerPosition, ResourceDescription, UnitShopDescription, ArmyDescription, MapObjectData, MapStartData, PositionToCellData};
    use heroes::utils::{Vec2, Vec2Trait, Direction, next_position};
    use heroes::map::Map;
    use super::IMapdataActions;
    use dojo::model::Model;


    fn upload_array<T, +Model<T>, +Copy<T>, +Drop<T>>(self: @ContractState, array: Array<T>) {
        let world = self.world_dispatcher.read();
        let mut i: u32 = 0;

        loop {
            if i == array.len() {
                break ();
            }
            let cell = *array.at(i);
            set!(world, (cell));

            i = i + 1;
        };
    }

    // impl: implement functions specified in trait
    #[external(v0)]
    impl MapdataActionsImpl of IMapdataActions<ContractState> {
        fn upload_map(self: @ContractState, cells: Array<PositionToCellData>, mapObjects: Array<MapObjectData>, startPosition: Vec2) {
            let world = self.world_dispatcher.read();
            let player = get_caller_address();

            upload_array::<PositionToCellData>(self, cells);
            upload_array::<MapObjectData>(self, mapObjects);

            set!(world, (MapStartData {id: 0, position: startPosition}));
        }

        fn upload_descriptions(self: @ContractState, resources: Array<ResourceDescription>, unitShops: Array<UnitShopDescription>, armies: Array<ArmyDescription>) {
            let world = self.world_dispatcher.read();
            let player = get_caller_address();

            upload_array::<ResourceDescription>(self, resources);
            upload_array::<UnitShopDescription>(self, unitShops);
            upload_array::<ArmyDescription>(self, armies);
        }
    }
}
