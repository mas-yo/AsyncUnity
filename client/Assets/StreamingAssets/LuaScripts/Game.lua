
function OnStart()
    player = PutPlayer(2)
    rock = PutObject(1)
    mushroom = PutObject(2)
    -- enemy = PutObject("Enemy", 5, 5)

    return player
end

-- function OnCollision(entity1, entity2)
--     print("Collision detected between " .. entity1.name .. " and " .. entity2.name)
-- end

