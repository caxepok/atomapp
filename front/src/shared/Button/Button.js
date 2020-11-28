import React from 'react'
import { NavLink } from 'react-router-dom'
import classes from 'classnames'
import './button.scss'

const Button = function (props) {
  const { onClick, children, small, light, to, disabled } = props 
  const buttonProps = {
    onClick: onClick,
    className: classes('button', small && '-small', light && '-light', disabled && '-disabled')
  }
  
  return to 
    ? <NavLink
      to={to}
      {...buttonProps}
    >
      <span>{children}</span>
    </NavLink>
    : <button
      type='button'
      {...buttonProps}
    >
      <span>{children}</span>
    </button>
}

export default Button