use starknet::ContractAddress;
use heroes::utils::{Vec2};

#[derive(Model, Copy, Drop, Serde)]
struct PlayerPosition {
    #[key]
    player: ContractAddress,
    position: Vec2
}

#[derive(Model, Copy, Drop, Serde)]
struct PlayerBalance {
    #[key]
    player: ContractAddress,
    gold: u32
}

#[derive(Model, Copy, Drop, Serde)]
struct PlayerUnits {
    #[key]
    player: ContractAddress,
    #[key]
    unit_id: u32,
    count: u32
}

#[derive(Model, Copy, Drop, Serde)]
struct PositionToCellData {
    #[key]
    position: Vec2,
    tile_id: u32,
    object_id: u32
}

#[derive(Model, Copy, Drop, Serde)]
struct MapStartData {
    #[key]
    id: u32,
    
    position: Vec2,
}

#[derive(Model, Copy, Drop, Serde)]
struct MapObjectData {
    #[key]
    id: u32,
    
    position: Vec2,
    description_id: u32
}

#[derive(Model, Copy, Drop, Serde)]
struct MapObject {
    #[key]
    player: ContractAddress,
    #[key]
    id: u32,
    
    position: Vec2,
    description_id: u32

    // last_day_used: u32
}

#[derive(Serde, Copy, Drop, Introspect)]
enum MapObjectType {
    Empty,
    Obstacle,
    UnitShop,
    Resource
}

#[derive(Model, Copy, Drop, Serde)]
struct MapObjectDescription {
    #[key]
    id: u32,
    obj_type: MapObjectType
}

#[derive(Model, Copy, Drop, Serde)]
struct UnitShopDescription {
    #[key]
    id: u32,
    unit_id: u32,
    price: u32
}

#[derive(Model, Copy, Drop, Serde)]
struct ResourceDescription {
    #[key]
    id: u32,
    resource_id: u32,
    count: u32
}

#[derive(Model, Copy, Drop, Serde)]
struct ArmyDescription {
    #[key]
    id: u32,

    unit1_id: u32,
    unit1_count: u32,
    unit2_id: u32,
    unit2_count: u32,
    unit3_id: u32,
    unit3_count: u32,
    unit4_id: u32,
    unit4_count: u32,
    unit5_id: u32,
    unit5_count: u32,
}