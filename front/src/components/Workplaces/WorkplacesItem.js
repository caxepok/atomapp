import React, { useState, useEffect } from 'react'
import { useDispatch } from 'react-redux'
import classes from 'classnames'
import { getWorkplaceUsers } from '../../reducers/data'
import './workplaces.scss'

const WorkplacesItem = function (props) {
  const { id, name, children } = props
  const [isExpanded, setExpanded] = useState(false)
  const [ isSelected, setSelected ] = useState(false)
  const dispatch = useDispatch()
  
  const handleClick = function () {
    if (children && children.length > 0) {
      setExpanded(!isExpanded)
    } 
    setSelected(true)
    dispatch(getWorkplaceUsers(id, name))
  }
  
  useEffect(() => {
    document.addEventListener('mousedown', () => setSelected(false))
  }, [] )

  return <li>
    {name && <button 
      type='button' 
      className={classes(
        'workplaces-link', 
        isSelected && '-selected', 
        children && children.length > 0 &&  '-expandable',
        isExpanded && '-expanded'
      )} 
      onClick={handleClick}
    >
      {name}
    </button> }
    {isExpanded && children && children.length > 0 && <ul>
      { children.map(item => <WorkplacesItem key={item.id} {...item} />)}
    </ul>}
  </li>
}

export default WorkplacesItem