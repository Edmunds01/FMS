declare global {
  interface Number {
    toEurFormat(): string
  }
}

Number.prototype.toEurFormat = function () {
  return new Intl.NumberFormat('de-DE', { style: 'currency', currency: 'EUR' }).format(
    this.valueOf(),
  )
}
