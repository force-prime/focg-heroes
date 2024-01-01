use heroes::utils::{Direction, Vec2};

// define the interface
#[starknet::interface]
trait IActions<TContractState> {
    fn start_game(self: @TContractState);
    fn move(self: @TContractState, direction: Direction);
    fn pickup(self: @TContractState, position: Vec2);
    fn purchase_unit(self: @TContractState, position: Vec2, count: u32);
    fn fight(self: @TContractState, position: Vec2);
}

// dojo decorator
#[dojo::contract]
mod actions {
    use starknet::{ContractAddress, get_caller_address};
    use heroes::models::{PlayerPosition, MapStartData};
    use heroes::utils::{Vec2, Direction, next_position};
    use heroes::map::Map;
    use heroes::mapObjects::MapObjects;
    use super::IActions;

    #[event]
    #[derive(Drop, starknet::Event)]
    enum Event {
        Moved: Moved,
    }

    // declaring custom event struct
    #[derive(Drop, starknet::Event)]
    struct Moved {
        player: ContractAddress,
        direction: Direction
    }

    // impl: implement functions specified in trait
    #[external(v0)]
    impl ActionsImpl of IActions<ContractState> {
        fn start_game(self: @ContractState) {
            let world = self.world_dispatcher.read();
            let player = get_caller_address();

            let startPosition = get!(world, 0, (MapStartData));
            set!(world, (PlayerPosition { player: player, position: startPosition.position }) )
        }

        fn move(self: @ContractState, direction: Direction) {
            let world = self.world_dispatcher.read();
            let player = get_caller_address();

            let mut playerPosition = get!(world, player, (PlayerPosition));
            let newPosition = next_position(playerPosition.position, direction);

            assert(Map::is_valid_map_position(world, newPosition), 'Position outside map');
            assert(Map::is_walkable_position(world, newPosition), 'Not walkable position');

            playerPosition.position = newPosition;

            set!(world, (playerPosition));

            emit!(world, Moved { player, direction });
        }

        fn pickup(self: @ContractState, position: Vec2) {
            let world = self.world_dispatcher.read();
            let player = get_caller_address();

            let playerPosition = get!(world, player, (PlayerPosition));

            assert(Map::is_adjacent_position(playerPosition.position, position), 'Not adjacent');

            let mapObject = Map::get_object_at_pos(world, player, position);
            match mapObject {
                Option::Some(obj) => MapObjects::pickup_resource(world, player, obj),
                Option::None => assert(1 == 0, 'No map object')
            }
        }

        fn purchase_unit(self: @ContractState, position: Vec2, count: u32) {
            let world = self.world_dispatcher.read();
            let player = get_caller_address();

            let playerPosition = get!(world, player, (PlayerPosition));

            assert(Map::is_adjacent_position(playerPosition.position, position), 'Not adjacent');

            let mapObject = Map::get_object_at_pos(world, player, position);
            match mapObject {
                Option::Some(obj) => MapObjects::buy_units(world, player, obj, count),
                Option::None => assert(1 == 0, 'No map object')
            }
        }

        fn fight(self: @ContractState, position: Vec2) {
            let world = self.world_dispatcher.read();
            let player = get_caller_address();

            let playerPosition = get!(world, player, (PlayerPosition));

            assert(Map::is_adjacent_position(playerPosition.position, position), 'Not adjacent');

            let mapObject = Map::get_object_at_pos(world, player, position);
            match mapObject {
                Option::Some(obj) => MapObjects::fight(world, player, obj),
                Option::None => assert(1 == 0, 'No map object')
            }
        }
    }
}

#[cfg(test)]
mod tests {
    use starknet::class_hash::Felt252TryIntoClassHash;

    // import world dispatcher
    use dojo::world::{IWorldDispatcher, IWorldDispatcherTrait};

    // import test utils
    use dojo::test_utils::{spawn_test_world, deploy_contract};

    // import models
    use heroes::models::{player_position};
    use heroes::models::{PlayerPosition};

    // import actions
    use super::{actions, IActionsDispatcher, IActionsDispatcherTrait};

    #[test]
    #[available_gas(30000000)]
    fn test_move() {
        // caller
        let caller = starknet::contract_address_const::<0x0>();

        // models
        let mut models = array![player_position::TEST_CLASS_HASH];

        // deploy world with models
        let world = spawn_test_world(models);

        // deploy systems contract
        let contract_address = world
            .deploy_contract('salt', actions::TEST_CLASS_HASH.try_into().unwrap());
        let actions_system = IActionsDispatcher { contract_address };

        // call spawn()
        actions_system.start_game();
    }
}
