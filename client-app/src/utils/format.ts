declare global {
  interface Number {
    toEurFormat(): string;
  }
  interface String {
    toNumberFromEurFormat(): number;
  }
}

Number.prototype.toEurFormat = function () {
  return new Intl.NumberFormat("de-DE", { style: "currency", currency: "EUR" }).format(
    this.valueOf(),
  );
};

String.prototype.toNumberFromEurFormat = function (): number {
  const numericString = this.replace(/[^\d,-]/g, "").replace(",", ".");
  return parseFloat(numericString);
};
