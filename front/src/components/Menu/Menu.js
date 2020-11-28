import React from 'react'
import { NavLink } from 'react-router-dom'
import './menu.scss'
import { useSelector } from 'react-redux'

const Menu = function () {
  const userRole = useSelector(({ user }) => user.role)
  
  return <div className='menu'>
    <NavLink className='menu-link' to='/inbox'>
      Входящие
    </NavLink>
    {userRole !== 2 && <NavLink className='menu-link' to='/outbox'>
      Исходящие
    </NavLink> }
  </div>
}

export default Menu