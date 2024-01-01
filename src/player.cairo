use heroes::models::{PlayerBalance, PlayerUnits};
use heroes::utils::{Vec2};
use starknet::ContractAddress;
use dojo::world::{IWorldDispatcher, IWorldDispatcherImpl};


#[generate_trait]
impl Player of PlayerTraits {
    fn subtract_gold(world: IWorldDispatcher, player: ContractAddress, count: u32) {
        let mut currentBalance = get!(world, (player), (PlayerBalance));
        let goldBalance = currentBalance.gold;

        assert(goldBalance >= count, 'not enough gold');

        currentBalance.gold -= count;
        set!(world, (currentBalance));
    }

    fn add_gold(world: IWorldDispatcher, player: ContractAddress, count: u32) {
        let mut currentBalance = get!(world, (player), (PlayerBalance));

        currentBalance.gold += count;
        set!(world, (currentBalance));
    }


    fn add_units(world: IWorldDispatcher, player: ContractAddress, unit_id: u32, count: u32) {
        let mut currentCount = get!(world, (player, unit_id), (PlayerUnits));
        
        currentCount.count += count;
        set!(world, (currentCount));
    }
} 