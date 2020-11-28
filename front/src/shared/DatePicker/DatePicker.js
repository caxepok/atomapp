import React, { useState, useEffect } from 'react'
import ReactDatePicker from 'react-datepicker'
import { formatISO } from 'date-fns'
import 'react-datepicker/dist/react-datepicker.css'
import './date-picker.scss'


const DatePicker = function (props) {
  const { value, onChange, disabled, name } = props
  const [ selected, setSelected ] = useState(0)
  
  const handleChange = function (date) {
    onChange(formatISO(date), name)
  }
  
  useEffect(function () {
    if (value) {
      const selected = new Date(value)
      setSelected(selected)
    }
  }, [value])
  
  return <>
    <ReactDatePicker
      selected={selected}
      onChange={handleChange}
      disabled={disabled}
      dateFormat={'dd.MM.yyyy'}
    />
  </>
}

export default DatePicker