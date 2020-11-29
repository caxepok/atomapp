import React from 'react'
import { NavLink, useRouteMatch } from 'react-router-dom'
import './menu.scss'
import { useSelector } from 'react-redux'
import { Button } from '../../shared'

const Menu = function () {
  const userRole = useSelector(({ user }) => user.role)
  const match = useRouteMatch({ path: '*'})
  
  return <div className='menu'>
    <NavLink className='menu-link' to='/inbox'>
      Входящие
    </NavLink>
    {userRole !== 2 && <>
      <NavLink className='menu-link' to='/outbox'>
        Исходящие
      </NavLink>
      <NavLink className='menu-link' to='/summary'>
        Аналитика
      </NavLink>
      <NavLink className='menu-link' to='/processes'>
        Бизнес-процессы
      </NavLink>
      <Button small to={`${match.url}/@new-task`}>
        Создать задачу
      </Button>
    </>}
  </div>
}

export default Menu